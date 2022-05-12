
openssl req -x509 -newkey rsa:4096 -sha256 -days 365 -nodes -keyout CsPyMudServer.key -out CsPyMudServer.crt -subj "/CN=CsPyMudServer" -extensions v3_ca  -extensions v3_req -config config.cfg

openssl x509 -noout -text -in CsPyMudServer.crt

openssl pkcs12 -export -out CsPyMudServer.pfx -inkey CsPyMudServer.key -in CsPyMudServer.crt
