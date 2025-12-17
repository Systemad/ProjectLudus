$models = Get-ChildItem -Path "models/marts/core/*.sql"

foreach ($file in $models) {
    $modelName = $file.BaseName
    Write-Host "Generating clean YAML for $modelName..."
    
    # Run codegen and only keep lines starting from 'version:'
    $rawYaml = dbt --quiet run-operation codegen.generate_model_yaml --args "{'model_names': ['$modelName']}"
    $cleanYaml = $rawYaml | Where-Object { $_ -match 'version:|models:|name:|description:|columns:' }
    
    $cleanYaml | Out-File -FilePath "models/marts/$modelName.yml" -Encoding utf8
}