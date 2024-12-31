export class Encryptor {


  constructor() {

    }
    deriveKey(password: string, salt: string) {
        return password + salt;

    }
    getKey() {
        return 'key';
    }
}
