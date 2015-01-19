SET @host = hostname 
FOR /F "tokens=* USEBACKQ" %%F IN (`hostname`) DO (
SET var=%%F
)
ECHO %var%
.\CompiledGameClient\GameClient %var% 30000