param(
	[string]$ExchangeUsername = '',
	[string]$ExchangePassword = ''
)

Write-Information "Script 'DeploySolution' started."

# STEP1 : Get remote session
# User credentials
$securePassword = ConvertTo-SecureString $ExchangePassword -AsPlainText -Force
$UserCredential = New-Object System.Management.Automation.PSCredential($ExchangeUsername, $securePassword)
$Session = New-PSSession -ConfigurationName Microsoft.Exchange -ConnectionUri https://ps.protection.outlook.com/powershell-liveid/ -Credential $UserCredential -Authentication Basic -AllowRedirection

# Open session
Import-PSSession $Session

# STEP2: Create Quarantine rule.
#New-TransportRule -name DroitDeconnexionMatin -quarantine:$true -ActivationDate '16:00:00' -ExpiryDate '00:00:00'
#New-TransportRule -name DroitDeconnexionSoir -quarantine:$true -ActivationDate '00:00:00' -ExpiryDate '07:00:00'
New-TransportRule -name DroitDeconnexionTest -quarantine:$true -ActivationDate '00:00:00' -ExpiryDate '23:59:59' -Confirm:$false

# Final step : Disconnection
Remove-PSSession $Session
