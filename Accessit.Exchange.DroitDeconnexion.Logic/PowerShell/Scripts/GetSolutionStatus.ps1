param(
	[string]$ExchangeUsername = '',
	[string]$ExchangePassword = ''
)

Write-Information "Script 'GetSolutionStatus' started."


# STEP1 : Get remote session
# User credentials
$securePassword = ConvertTo-SecureString $ExchangePassword -AsPlainText -Force
$UserCredential = New-Object System.Management.Automation.PSCredential($ExchangeUsername, $securePassword)
$Session = New-PSSession -ConfigurationName Microsoft.Exchange -ConnectionUri https://ps.protection.outlook.com/powershell-liveid/ -Credential $UserCredential -Authentication Basic -AllowRedirection
$SolutionStatus = 'Non deployé'

# Open session
Import-PSSession $Session

# STEP2: Try to get Quarantine rule to get their status.
#$rule = Get-TransportRule -Identity DroitDeconnexionMatin
#$rule2 = Get-TransportRule -Identity DroitDeconnexionSoir
#if ($rule.State -eq "Enabled" -and $rule2.State -eq "Enabled") {
#	$SolutionStatus = 'Actif'
#}
#elseif ($rule.State -eq "Disabled" -and $rule2.State -eq "Disabled") {
#	$SolutionStatus = 'Déployé mais désactivé'
#}
#else {
#	$SolutionStatus = 'Undefined'
#}

$rule = Get-TransportRule -Identity DroitDeconnexionTest
if ($rule.State -eq "Enabled") {
	$SolutionStatus = 'Actif'
}
elseif ($rule.State -eq "Disabled") {
	$SolutionStatus = 'Déployé mais désactivé'
}
else {
	$SolutionStatus = 'Undefined'
}

# Final step : Disconnection
Remove-PSSession $Session

return "Result:$SolutionStatus"