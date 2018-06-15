param (
    [Parameter(Mandatory = $true)]
    [string]
    $Action,

    [Parameter(Mandatory = $true)]
    [string]
    $VMName
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
    
    $vm = Get-AzureRMVM | Where-Object Name -eq $VMName
    switch ($Action) {
        "Start" {
            Start-AzureRmVM -ResourceGroupName $vm.ResourceGroupName -Name $vm.Name
        }
        "Stop" {
            Stop-AzureRmVM -ResourceGroupName $vm.ResourceGroupName -Name $vm.Name -Force
        }
        "Restart" {
            Restart-AzureRmVM -ResourceGroupName $vm.ResourceGroupName -Name $vm.Name
        }
        Default {
            Write-Error "$Action - action not found"
        }
    }
}
catch {
    Write-Error -Message $_.Exception
    throw $_.Exception
}
