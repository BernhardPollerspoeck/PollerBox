

# Setup 

---

### Get Docker Compose file
```
curl -L -o docker-compose.yml https://raw.githubusercontent.com/BernhardPollerspoeck/PollerBox/master/docker-compose.yml
```


docker build . -t bernhardpollerspoeck/pollerbox:latest
docker push bernhardpollerspoeck/pollerbox

---

### Configure the docker-compose.yml file to your needs
- Set the audio device to your needs. Run `arecord -l` to get a list of available audio devices. 
```yaml
AUDIODEV=hw:1,0 
```
- Remove POLLER_BOX_USE_SPI if you don't want to use the SPI interface
---

### Start the PollerBox
```
docker-compose up -d
```
