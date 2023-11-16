# Avvio di app tramite container con GUI

## Requisiti

- Docker
- WSL2 (Windows Subsystem for Linux 2)

## Comando

```dockerfile
FROM ubuntu:22.04
RUN apt update -y && apt install -y gedit
CMD ["gedit"]
```

``` powershell
docker run --privileged -it -v /run/desktop/mnt/host/wslg/.X11-unix:/tmp/.X11-unix -v /run/desktop/mnt/host/wslg:/mnt/wslg -e DISPLAY=:0 -e WAYLAND_DISPLAY=wayland-0 -e XDG_RUNTIME_DIR=/mnt/wslg/runtime-dir -e PULSE_SERVER=/mnt/wslg/PulseServer <image>
```

o in alternativa con docker-compose

```yaml
version: '3.3'
services:
    guitest:
        volumes:
            - '/run/desktop/mnt/host/wslg/.X11-unix:/tmp/.X11-unix'
            - '/run/desktop/mnt/host/wslg:/mnt/wslg'
        environment:
            - 'DISPLAY=:0'
            - WAYLAND_DISPLAY=wayland-0
            - XDG_RUNTIME_DIR=/mnt/wslg/runtime-dir
            - PULSE_SERVER=/mnt/wslg/PulseServer
        image: guitest
```

```powershell
docker compose build -t <image name> <Dockerfile path>
docker-compose up
```

## Altri comandi da aggiungere

Passaggi per riprodurre:

1. seguire installazione di UnityHub (quella contenuta nello script)

2. installare fuse `apt install fuse`

3. installare firefox (necessario per autenticazione): `apt install firefox`

4. avviare UnityHub: `.\UnityHub.AppImage`

5. procedere con il login

6. **Se non funziona il login**: aprire il codice sorgente della pagina e recuperare il link `unityhub:\\` e incollarlo dopo il comando di avvio `.\UnityHub.AppImage unityhub:\\...` e avviare

7. **Da testare**: copiare il comando sotto e vedere se il collegamento funziona in automatico

   ```bash
   xdg-mime default appimagekit-unityhub.desktop x-scheme-handler/unityhub
   ```

  [Unityci](www.unityci.com). 

