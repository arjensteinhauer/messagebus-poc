if exist MB.template.json del MB.template.json
if exist MB.parameters.json del MB.parameters.json
if exist bin del /s /q bin

call arm-template-merge **\*.template.json MB.template.json
call arm-template-merge **\*.parameters.json MB.parameters.json
