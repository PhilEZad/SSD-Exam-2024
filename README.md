# Secure Software Development Exam 2024, EASV.

- [Secure Software Development Exam](#ssd-exam)
  - [How to run the application](#how-to-run-the-application)


## How to run the application

#### STEP 1: Docker Install
Download and install [docker](https://docs.docker.com/engine/install/ "docker").

#### STEP 2: Certificate Setup
Open PowerShell and run the generate-certs.ps1 script found in the root directory.

#### STEP 3: .Env Setup
In the root directory, create a file named .env with the following structure:
.env
```
#Database Password
SA_PASSWORD=<Database Password>

#Certificate Password
CERT_PASS=<TLS Passwrod>
```

#### STEP 4: Vault Steup
The project contains two "applications", the frontend and backend, as well a a Vault.
Navitagte to the root directory and run:
```
docker compose build; docker compose up -d
```
The backened will fail.

Navigate to Vault, running on [localhost:8200](http://localhost:8200 "localhost:8200").
Follow the instructions presented.

Once you have a token and keyshards, create a file in the root folder called vault_keys.json with the following structure:
```
{
  "Token": "<access-token>"
  "Keys": [
    "<unseal-key-1>",
    "<unseal-key-2>",
    "<unseal-key-3>",
    "<unseal-key-4>",
    "<unseal-key-5>"
  ],
}
```

Once that is done, create a secret engine by the name of kv—ensure it is running secret engine 2.0.

Create three new secrets. Each with the name of jwt, hash and database. Ensure it follows the key-value pairing with the key name of **key**.

For the database, make the value is the entirety of the connection string to the database — ensure the password matches the password set in the .env file.
```
jwt:
key - secret
hash:
key - secret
database:
key - connection string
```

#### Step 5: Run the application
Restart the application with the following command from the rood directory:
```
docker compose down; docker compose up -d
```
Once all containers are booted and ready, the application can be accessed on [localhost:8000](https://localhost:8000 "localhost:8000").
