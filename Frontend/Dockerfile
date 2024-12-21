# Stage 1: Build Angular Application
FROM node:alpine AS build

# Set the working directory inside the container
WORKDIR /app

# Copy package.json and package-lock.json to install dependencies
COPY package.json package-lock.json ./

# Install project dependencies
RUN npm install

# Copy all Angular application files
COPY . .

# Build the Angular application for production
RUN npm run build --prod

# Stage 2: Run Angular Application
FROM node:alpine AS run

# Set working directory for serving the app
WORKDIR /app

# Install a simple HTTP server globally
RUN npm install -g http-server

# Copy the built Angular app from the build stage
COPY --from=build /app/dist /app/dist

# Expose the application on port 8080
EXPOSE 8080

# Serve the Angular app using HTTP server
CMD ["http-server", "/app/dist", "-p", "8080"]
