param(
	[string]$ExchangeUsername = '',
	[string]$ExchangePassword = '',
	[string]$QuarantineEmails = 'NONE'
)

Write-Information "Script 'ReleaseQuarantinedMails' started."

# STEP1 : Get remote session
# User credentials
$securePassword = ConvertTo-SecureString $ExchangePassword -AsPlainText -Force
$UserCredential = New-Object System.Management.Automation.PSCredential($ExchangeUsername, $securePassword)
$Session = New-PSSession -ConfigurationName Microsoft.Exchange -ConnectionUri https://ps.protection.outlook.com/powershell-liveid/ -Credential $UserCredential -Authentication Basic -AllowRedirection

# Open session
Import-PSSession $Session

# STEP2: Get Quarantined mails .
$emails = Get-QuarantineMessage -QuarantineTypes TransportRule
if ($QuarantineEmails -eq "ALL")
{
	$emails = $emails
}
elseif ($QuarantineEmails -eq "NONE")
{
	$emails = $NULL
}
else {
	$emailsToRelease = $QuarantineEmails.Split(';')
	$emails = $emails | Where-Object { $_.MessageId -in $emailsToRelease}
}
# Release emails.
$emails | Release-QuarantineMessage -ReleaseToAll -Confirm:$false
# Remove them from the quarantine zone.
$emails | Delete-QuarantineMessage -Confirm:$false

# Final step : Disconnection
Remove-PSSession $Session
