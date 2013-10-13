
# Use the same name of filename.
workflow SampleWorkflow {
   Write-Output -InputObject "Good Morning, World."
   Start-Sleep -Seconds 5
   Checkpoint-Workflow
   Write-Warning -Message "Hello, World."
   Start-Sleep -Seconds 5
   Write-Output -InputObject "Good Evening, World."
}
