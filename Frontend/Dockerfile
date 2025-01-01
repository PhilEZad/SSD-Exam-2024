### STAGE 1:BUILD ###
# Defining a node image to be used as giving it an alias of "build"
# Which version of Node image to use depends on project dependencies
# This is needed to build and compile our code
# while generating the docker image
FROM node:alpine AS build
# Create a Virtual directory inside the docker image
WORKDIR /dist/src/app
# Copy files to virtual directory
# COPY package.json package-lock.json ./
# Run command in Virtual directory
RUN npm cache clean --force
# Copy files from local machine to virtual directory in docker image
COPY . .
RUN npm install
RUN npm run build --prod


### STAGE 2:RUN ###
# Defining nginx image to be used
FROM nginx:latest AS ngi
# Copying compiled code and nginx config to different folder
# NOTE: This path may change according to your project's output folder
COPY --from=build /dist/src/app/dist/write-on-frontend /usr/share/nginx/html
COPY /nginx.conf  /etc/nginx/conf.d/default.conf

# Generate dummy SSL certificate and key
RUN mkdir -p /etc/nginx/ssl && \
    openssl req -x509 -nodes -days 365 -newkey rsa:2048 \
    -keyout /etc/nginx/ssl/dummy.key \
    -out /etc/nginx/ssl/dummy.crt \
    -subj "/CN=localhost"

# Exposing a port, here it means that inside the container
# the app will be using Port 80 while running
EXPOSE 80 443
