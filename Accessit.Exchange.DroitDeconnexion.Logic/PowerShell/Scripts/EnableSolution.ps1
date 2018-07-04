param(
	[string]$ExchangeUsername = '',
	[string]$ExchangePassword = ''
)

Write-Information "Script 'EnableSolution' started."

# STEP1 : Get remote session
# User credentials
$securePassword = ConvertTo-SecureString $ExchangePassword -AsPlainText -Force
$UserCredential = New-Object System.Management.Automation.PSCredential($ExchangeUsername, $securePassword)
$Session = New-PSSession -ConfigurationName Microsoft.Exchange -ConnectionUri https://ps.protection.outlook.com/powershell-liveid/ -Credential $UserCredential -Authentication Basic -AllowRedirection

# Open session
Import-PSSession $Session

# STEP2: Disable the rule.
#Enable-TransportRule -Identity DroitDeconnexionMatin -Confirm:$false
#Enable-TransportRule -Identity DroitDeconnexionSoir -Confirm:$false
Enable-TransportRule -Identity DroitDeconnexionTest -Confirm:$false

# Final step : Disconnection
Remove-PSSession $Session
