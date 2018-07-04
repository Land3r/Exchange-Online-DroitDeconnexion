param(
	[string]$ExchangeUsername = '',
	[string]$ExchangePassword = ''
)

Write-Information "Script 'GetQuarantinedMails' started."

# STEP1 : Get remote session
# User credentials
$securePassword = ConvertTo-SecureString $ExchangePassword -AsPlainText -Force
$UserCredential = New-Object System.Management.Automation.PSCredential($ExchangeUsername, $securePassword)
$Session = New-PSSession -ConfigurationName Microsoft.Exchange -ConnectionUri https://ps.protection.outlook.com/powershell-liveid/ -Credential $UserCredential -Authentication Basic -AllowRedirection

# Open session
Import-PSSession $Session

# STEP2: Get Quarantined mails .
$emails = Get-QuarantineMessage -QuarantineTypes TransportRule
$emails | ForEach-Object {
	$id = $_.MessageId
	$sender = $_.SenderAddress
	Write-Output "Result:$id;$sender"
}

# Final step : Disconnection
Remove-PSSession $Session
