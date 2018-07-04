param(
	[string]$ExchangeUsername = '',
	[string]$ExchangePassword = ''
)

Write-Information "Script 'RetractSolution' started."

# STEP1 : Get remote session
# User credentials
$securePassword = ConvertTo-SecureString $ExchangePassword -AsPlainText -Force
$UserCredential = New-Object System.Management.Automation.PSCredential($ExchangeUsername, $securePassword)
$Session = New-PSSession -ConfigurationName Microsoft.Exchange -ConnectionUri https://ps.protection.outlook.com/powershell-liveid/ -Credential $UserCredential -Authentication Basic -AllowRedirection

# Open session
Import-PSSession $Session

# STEP2: Remove Quarantine rule.
#Remove-TransportRule -identity DroitDeconnexionMatin -Confirm:$false
#Remove-TransportRule -identity DroitDeconnexionSoir -Confirm:$false
Remove-TransportRule -identity DroitDeconnexionTest -Confirm:$false

# Final step : Disconnection
Remove-PSSession $Session