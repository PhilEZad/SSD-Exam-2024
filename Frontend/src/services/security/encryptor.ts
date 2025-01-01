import {KeyMaker} from './key-maker';

export class Encryptor {

  constructor() {}


  private static async getEncryptionKey(): Promise<CryptoKey | null> {
    return await KeyMaker.getKey();
  }

  // Encrypt data
  static async encrypt(data: string): Promise<string> {
    const key = await this.getEncryptionKey();
    if (!key) {
      throw new Error('Encryption key not available');
    }

    const encoder = new TextEncoder();
    const encodedData = encoder.encode(data);

    // Generate a secure random IV (16 bytes for enhanced strength)
    const iv = crypto.getRandomValues(new Uint8Array(16));

    const cipherText = await crypto.subtle.encrypt(
      {
        name: 'AES-GCM',
        iv,
      },
      key,
      encodedData
    );

    // Combine IV and cipherText into one buffer
    const combinedBuffer = new Uint8Array(iv.byteLength + cipherText.byteLength);
    combinedBuffer.set(iv, 0);
    combinedBuffer.set(new Uint8Array(cipherText), iv.byteLength);

    // Convert combined buffer to base64 for storage or transmission
    return btoa(String.fromCharCode(...combinedBuffer));
  }

  // Decrypt data
  static async decrypt(cipherText: string): Promise<string> {
    const key = await this.getEncryptionKey();
    if (!key) {
      throw new Error('Encryption key not available');
    }

    // Decode base64 into a buffer
    const combinedBuffer = new Uint8Array(
      atob(cipherText)
        .split('')
        .map((char) => char.charCodeAt(0))
    );

    // Extract the IV (16 bytes) and encrypted data
    const iv = combinedBuffer.slice(0, 16);
    const encryptedData = combinedBuffer.slice(16);

    const decryptedBuffer = await crypto.subtle.decrypt(
      {
        name: 'AES-GCM',
        iv,
      },
      key,
      encryptedData
    );

    // Decode the decrypted buffer into a string
    const decoder = new TextDecoder();
    return decoder.decode(decryptedBuffer);
  }


}
