param (
    [Parameter(Mandatory = $true)]
    [string]
    $GroupName,

    [Parameter(Mandatory = $true)]
    [string]
    $UserName
)

$connectionName = "AzureRunAsConnection"
try {
    $servicePrincipalConnection = Get-AutomationConnection -Name $connectionName
    "Logging in to Azure Active Directory" 
    Connect-AzureAD `
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
        Write-Error -Message $_.Exception
        throw $_.Exception
    }
}

try {
    $adGroup = Get-AzureADGroup -Filter "DisplayName eq '$GroupName'"
    $adUser = Get-AzureADUser -Filter "userPrincipalName eq '$UserName'"
    Write-Output "Adds a $UserName member to $GroupName."
    Add-AzureADGroupMember -ObjectId $adGroup.ObjectId -RefObjectId $adUser.ObjectId
}
catch {
    Write-Error -Message $_.Exception
    throw $_.Exception
}
