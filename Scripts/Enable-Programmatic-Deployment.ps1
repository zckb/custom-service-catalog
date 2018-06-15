param (
    [Parameter(Mandatory = $true)]
    [string]
    $Publisher,
    [Parameter(Mandatory = $true)]
    [string]
    $Product,
    [Parameter(Mandatory = $true)]
    [string]
    $Name
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
        Write-Output $ErrorMessage
        throw $ErrorMessage
    }
    else {
        Write-Output -Message $_.Exception
        throw $_.Exception
    }
}

try {
    $agreementTerms = Get-AzureRmMarketplaceTerms -Publisher $Publisher -Product $Product -Name $Name
    Set-AzureRmMarketplaceTerms -Publisher $Publisher -Product $Product -Name $Name -Terms $agreementTerms -Accept
}
catch {
    Write-Error -Message $_.Exception
    throw $_.Exception
}