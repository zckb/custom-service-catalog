Param (
    [string]$userName,
    [String]$userPassword
)

NET USER $userName $userPassword /ADD

NET LOCALGROUP "Remote Desktop Users" $userName /ADD