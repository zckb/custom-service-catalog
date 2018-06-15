param (
    [Parameter(Mandatory = $true)]
    [string]
    $billingPeriod
)

$connectionName = "AzureRunAsConnection"
try {
    $servicePrincipalConnection = Get-AutomationConnection -Name $connectionName

    "Logging in to Azure..."
    Add-AzureRmAccount `
        -ServicePrincipal `
        -TenantId $servicePrincipalConnection.TenantId `
        -ApplicationId $servicePrincipalConnection.ApplicationId `
        -CertificateThumbprint $servicePrincipalConnection.CertificateThumbprint 
}
catch {
    if (!$servicePrincipalConnection) {
        $ErrorMessage = "Connection $connectionName not found."
        throw $ErrorMessage
    }
    else {
        Write-Output -Message $_.Exception
        throw $_.Exception
    }
}

try {
    $invoices = @{}
    switch ($billingPeriod) {
        "Latest" {
            $invoices = Get-AzureRmBillingInvoice -Latest
        }
        "All" {
            $invoices = Get-AzureRmBillingInvoice -GenerateDownloadUrl
        }
        Default {
            Write-Output "$billingPeriod - incorrect period"
        }
    }

    foreach ($invoice in $invoices) {
        Write-Output $invoice.Name / $invoice.DownloadUrl 
    }
}
catch {
    Write-Output -Message $_.Exception
    throw $_.Exception
}
