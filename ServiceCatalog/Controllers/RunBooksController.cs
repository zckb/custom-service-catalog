// -----------------------------------------------------------------------
// <copyright file="RunBooksController.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using ServiceCatalog.Common.Helpers;

namespace ServiceCatalog.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using BusinessLogic.Client;
    using Common.Constants;
    using Models;
    using System;
    using System.Security.Claims;
    using System.Data.Entity;
    using Context;

    public class RunBooksController : BaseController
    {
        // GET: RunBooks
        public async Task<ActionResult> RunBooksView()
        {
            try
            {
                Log.Info("Start RunBooksController");
                var subscriptions = await new SubscriptionController().GetSubscriptions();
                var subscriptionId = subscriptions.FirstOrDefault()?.SubscriptionId;
                var token = await ServicePrincipal.GetAccessToken();
                var automationAccountClient = new RestApiClient();

                var automationAccountUri = string.Format(UriConstants.GetAutomationAccounts, Url.Encode(subscriptionId));
                var automationAccounts = await automationAccountClient.CallGetListAsync<AutomationAccount>(automationAccountUri, token);
                var automationAccountsResult = automationAccounts.Result;

                var email = ClaimsPrincipal.Current.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name").Value;
                Log.Info($"RunBooksController Owner: {email}");
                List<Job> localJobs;
                using (var webAppContext = new WebAppContext())
                {
                    localJobs = await webAppContext.Jobs.ToListAsync();
                }
                Log.Info($"LocalJobs: {TemplateHelper.ToJson(localJobs)}");
                var jobList = new List<JobViewModel>();
                foreach (var account in automationAccountsResult)
                {
                    var jobAccountClient = new RestApiClient();
                    var jobsUrl = string.Format(UriConstants.GetJobs, account.Id);
                    var jobs = await jobAccountClient.CallGetListAsync<JobViewModel>(jobsUrl, token);
                    var jobsResult = jobs.Result;
                    foreach (var job in jobsResult)
                    {
                        var isUserOwner = localJobs.Any(j => j.Id == job.Properties.JobId && j.Owner == email);
                        Log.Info($"job status: {isUserOwner}");
                        if (UserRoleHelper.IsAdmin(email) || isUserOwner)
                        {
                            var jobOutputClient = new RestApiClient();
                            var jobOutputUrl = string.Format(UriConstants.GetJobOutput, job.Id);
                            var jobOutput = await jobOutputClient.CallGetText(jobOutputUrl, token);
                            var newJob = job;
                            newJob.Outputs = jobOutput;
                            jobList.Add(newJob);
                        }
                    }
                }

                return View(jobList);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Error";
                ViewBag.ErrorDetails = ex.Message;

                Log.Error(ex);

                return View("Error");
            }
        }
    }
}