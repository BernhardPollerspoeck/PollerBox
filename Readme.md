

## Setup 

- Check for Docker and Docker Compose
```bash
docker --version
docker-compose --version
```
- In case Docker is not found install it
```bash
sudo apt-get update
sudo apt-get install docker.io
```
- In case Docker Compose is not found install it
```bash
sudo apt-get update
sudo apt-get install docker-compose
```
- Get Docker Compose file
```
curl -L -o docker-compose.yml https://raw.githubusercontent.com/BernhardPollerspoeck/PollerBox/master/docker-compose.yml
```
