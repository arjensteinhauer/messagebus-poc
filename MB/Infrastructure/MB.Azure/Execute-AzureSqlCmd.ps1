[CmdletBinding()]
Param (
    [Parameter(Mandatory = $true)]
    [String] $SqlServerName,

    [Parameter(Mandatory = $true)]
    [String] $SqlDatabaseName,
    
    [Parameter(Mandatory = $true)]
    [String] $SqlToken,

    [Parameter(Mandatory = $true)]
    [ValidateSet('User','Group','ManagedIdentity')]
    [String] $LoginType,

    [Parameter(Mandatory = $true)]
    [String] $PrincipalName,

    [Parameter()]
    [ValidateSet('db_datareader','db_datawriter','db_denydatareader','db_denydatawriter','db_ddladmin','bulkadmin')]
    [string[]] $DatabaseRoles
)

switch ($LoginType) {
    "User" { 
        $idForSid = $(Get-AzADUser -UserPrincipalName $PrincipalName).Id 
    }
    "Group" { 
        $idForSid = $(Get-AzADGroup -DisplayName $PrincipalName).Id
    }
    "ManagedIdentity" {
        $idForSid = (Get-AzADServicePrincipal -DisplayName $PrincipalName).ApplicationId.Guid
    }
    Default { Write-Error "Unknown login type '$LoginType'." }
}

$sid = "0x" + [System.String]::Join('', ((New-Object System.Guid $idForSid).ToByteArray() | ForEach-Object { $_.ToString("X2") }))

try {
    $connection = New-Object System.Data.SqlClient.SqlConnection
    $connection.ConnectionString = "Data Source=tcp:$SqlServerName.database.windows.net,1433;Initial Catalog=$SqlDatabaseName"
    $connection.AccessToken = $SqlToken

    Write-Output "Creating SQL login for security principal of type '$LoginType' and name '$PrincipalName' and SID '$sid'."
    
    $userType = if ($LoginType -eq "Group") { "X" } else { "E" }

    $query += "IF NOT EXISTS(SELECT 1 FROM sys.database_principals WHERE name ='$PrincipalName')`n"
    $query += "BEGIN`n"
    $query += "    CREATE USER [$PrincipalName] WITH SID = $sid, TYPE = $userType;`n"
    $query += "END`n"

    foreach ($role in $DatabaseRoles) {
        $query += "ALTER ROLE $role ADD MEMBER [$PrincipalName];`n"  
    }

    Write-Output "Execute SQL-query : `n $query"

    $command = New-Object System.Data.SqlClient.SqlCommand
    $command.CommandText = $query
    $command.Connection = $connection

    $connection.Open()
    $command.ExecuteNonQuery()
}
catch {
    throw "Error: $($_.Exception.Message)"
}
finally {
    if ($connection.State -ne "Closed") {
        $connection.Close()
    }
}