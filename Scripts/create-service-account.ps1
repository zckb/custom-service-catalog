<#

.SYNOPSIS
This is a PowerShell script to create an Azure service principal

.DESCRIPTION
You can use the PowerShell script to create an Azure Active Directory Application and Service Principal

.EXAMPLE
.\create-service-account.ps1 `
    -subscriptionId "C57D7401-4409-4D74-A6DD-346DC1C7F3A0" `
    -tenantId "ADFB12FE-F2F4-4820-8CF0-49E5AF4CC95C" `
    -siteName "YOUR_SITE_NAME" `
    -sitePassword "SERVICE_ACCOUNT_PASSWORD" `
    -azureAccountName "YOUR_AZURE_ACCOUNT_NAME" `
    -azureAccountPassword "YOUR_AZURE_ACCOUNT_PASSWORD"

.NOTES
Run PowerShell script as Administrator

.LINK
https://logic2020.repositoryhosting.com/git_public/logic2020/svccat.git/blob_plain/refs/heads/master:/

#>

#Requires -RunAsAdministrator

param(
    [string] [Parameter(Mandatory = $true)]$subscriptionId,
    [string] [Parameter(Mandatory = $true)]$tenantId,
    [string] [Parameter(Mandatory = $true)]$siteName,
    [string] [Parameter(Mandatory = $true)]$sitePassword,
    [string] [Parameter(Mandatory = $true)]$azureAccountName,
    [string] [Parameter(Mandatory = $true)]$azureAccountPassword
)

Import-Module AzureRM.Resources

$url = "https://$siteName.azurewebsites.net/"
$replyUrl = "$url.auth/login/aad/callback"
$replyUrls = @($url, $replyUrl)
$endDate = (Get-Date).AddYears(4)

# Login to your Azure Subscription
Write-Output "Login to your Azure Subscription..."
$azureSecurePassword = ConvertTo-SecureString $azureAccountPassword -AsPlainText -Force
$psCred = New-Object System.Management.Automation.PSCredential($azureAccountName, $azureSecurePassword)
try {
    Login-AzureRmAccount -Credential $psCred -ErrorAction stop
    Set-AzureRMContext -SubscriptionId $subscriptionId -TenantId $tenantId -ErrorAction stop
}
catch {
    throw $_
}

# Create Application in Active Directory
Write-Output "Creating AAD application..."
try {
    $securePassword = ConvertTo-SecureString $sitePassword -AsPlainText -Force
    $azureAdApplication = New-AzureRmADApplication -DisplayName $siteName -HomePage $url -IdentifierUris $url -Password $securePassword -ReplyUrls $replyUrls -EndDate $endDate -ErrorAction stop
}
catch {
    throw $_
}

# Create the Service Principal
Write-Output "Creating AAD service principal..."
try {
    New-AzureRmADServicePrincipal -ApplicationId $azureAdApplication.ApplicationId -ErrorAction stop
    Start-Sleep -s 20 
}
catch {
    throw $_
}

# Assign the Service Principal the Contributor Role to the Subscription.
Write-Output "Assigning the Contributor role to the service principal..."
Try {
    Connect-AzureAD -Credential $psCred -TenantId $tenantId -SubscriptionId $subscriptionId
    New-AzureRmRoleAssignment -RoleDefinitionName Contributor -ServicePrincipalName $azureAdApplication.ApplicationId -ErrorAction Stop
}
Catch {
    throw $_
}


# Assign delegated permissions
Write-Output "Assigning the delegated permissions..."
try {
    $requiredResources = [System.Collections.Generic.List[Microsoft.Open.AzureAD.Model.RequiredResourceAccess]]::New()

    # Section 1 | Windows Azure Active Directory
    $requiredResourceAccess1 = [Microsoft.Open.AzureAD.Model.RequiredResourceAccess]::New()
    $requiredResourceAccess1.ResourceAppId = "00000002-0000-0000-c000-000000000000"

    $resourceAccess1_1 = [Microsoft.Open.AzureAD.Model.ResourceAccess]::New()
    $resourceAccess1_1.Id = "cba73afc-7f69-4d86-8450-4978e04ecd1a"
    $resourceAccess1_1.Type = "Scope"

    $resourceAccess1_2 = [Microsoft.Open.AzureAD.Model.ResourceAccess]::New()
    $resourceAccess1_2.Id = "311a71cc-e848-46a1-bdf8-97ff7156d8e6"
    $resourceAccess1_2.Type = "Scope"

    $resourceAccess1_3 = [Microsoft.Open.AzureAD.Model.ResourceAccess]::New()
    $resourceAccess1_3.Id = "5778995a-e1bf-45b8-affa-663a9f3f4d04"
    $resourceAccess1_3.Type = "Scope"

    $resourceAccess1_4 = [Microsoft.Open.AzureAD.Model.ResourceAccess]::New()
    $resourceAccess1_4.Id = "5778995a-e1bf-45b8-affa-663a9f3f4d04"
    $resourceAccess1_4.Type = "Role"

    $resourceAccess1_5 = [Microsoft.Open.AzureAD.Model.ResourceAccess]::New()
    $resourceAccess1_5.Id = "6234d376-f627-4f0f-90e0-dff25c5211a3"
    $resourceAccess1_5.Type = "Scope"

    $resourceAccess1_6 = [Microsoft.Open.AzureAD.Model.ResourceAccess]::New()
    $resourceAccess1_6.Id = "c582532d-9d9e-43bd-a97c-2667a28ce295"
    $resourceAccess1_6.Type = "Scope"

    $requiredResourceAccess1.ResourceAccess = $resourceAccess1_1, $resourceAccess1_2, $resourceAccess1_3, $resourceAccess1_4, $resourceAccess1_5, $resourceAccess1_6
    $requiredResources.Add($requiredResourceAccess1)

    # Section 2 | Windows Azure Service Management API
    $requiredResourceAccess2 = [Microsoft.Open.AzureAD.Model.RequiredResourceAccess]::New()
    $requiredResourceAccess2.ResourceAppId = "797f4846-ba00-4fd7-ba43-dac1f8f63013"

    $resourceAccess2_1 = [Microsoft.Open.AzureAD.Model.ResourceAccess]::New()
    $resourceAccess2_1.Id = "41094075-9dad-400e-a0bd-54e686782033"
    $resourceAccess2_1.Type = "Scope"

    $requiredResourceAccess2.ResourceAccess = $resourceAccess2_1
    $requiredResources.Add($requiredResourceAccess2);

    # Section 3 | Microsoft Graph
    $requiredResourceAccess3 = [Microsoft.Open.AzureAD.Model.RequiredResourceAccess]::New()
    $requiredResourceAccess3.ResourceAppId = "00000003-0000-0000-c000-000000000000"

    $resourceAccess3_1 = [Microsoft.Open.AzureAD.Model.ResourceAccess]::New()
    $resourceAccess3_1.Id = "7ab1d382-f21e-4acd-a863-ba3e13f7da61"
    $resourceAccess3_1.Type = "Role"

    $resourceAccess3_2 = [Microsoft.Open.AzureAD.Model.ResourceAccess]::New()
    $resourceAccess3_2.Id = "06da0dbc-49e2-44d2-8312-53f166ab848a"
    $resourceAccess3_2.Type = "Scope"

    $requiredResourceAccess3.ResourceAccess = $resourceAccess3_1, $resourceAccess3_2;
    $requiredResources.Add($requiredResourceAccess3);

    # Update Azure AD Application
    Set-AzureADApplication -ObjectId $azureAdApplication.ObjectId -RequiredResourceAccess $requiredResources

    # Generate a client secret
    $passwordCredential = New-AzureADApplicationPasswordCredential -ObjectId $azureAdApplication.ObjectId -StartDate $now -EndDate $endDate

    # Get Azure Tenant Domain Name
    $tenant = Get-AzureRmTenant

    # Display Output Parameters
    Write-Output ""
    Write-Output "====== Application Configs ======"
    Write-Output "Application Id            : $($azureAdApplication.ApplicationId)"
    Write-Output "Application Secret Key    : $($passwordCredential.Value)"
    Write-Output "Tenant Name               : $($tenant.Directory)"
    Write-Output "Site Name                 : $($siteName)"
}
catch {
    throw $_
}
