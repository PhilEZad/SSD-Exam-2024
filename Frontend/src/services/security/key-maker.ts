import {Injectable} from '@angular/core';
import {environment} from '../../environments/environment';
import {mergeMap, Observable} from 'rxjs';
import {fromPromise} from 'rxjs/internal/observable/innerFrom';

@Injectable({
  providedIn: 'root',
})
export class KeyMaker {

  constructor() {}

  static deriveAndStore$(input: string, salt: string): Observable<void> {
    return fromPromise(this.deriveKey(input, salt)).pipe(
      mergeMap((key) => fromPromise(this.storeKey(key))) // Chain the storage step
    );
  }

  /**
   * Derives a key from a string using PBKDF2.
   * @param input The input string (e.g., password or passphrase).
   * @param salt A unique salt for the derivation process.
   * @returns The derived key as a CryptoKey object.
   */
  static async deriveKey(input: string, salt: string): Promise<CryptoKey> {
    const encoder = new TextEncoder();
    const keyMaterial = await window.crypto.subtle.importKey(
      'raw',
      encoder.encode(input),
      { name: 'PBKDF2' },
      false,
      ['deriveKey']
    );

    return await window.crypto.subtle.deriveKey(
      {
        name: 'PBKDF2',
        salt: encoder.encode(salt),
        iterations: 100000,
        hash: 'SHA-256',
      },
      keyMaterial,
      {name: 'AES-GCM', length: 256},
      true,
      ['encrypt', 'decrypt']
    );
  }

  /**
   * Exports a CryptoKey to a raw binary format and encodes it as a base64 string for storage.
   * @param key The CryptoKey to export.
   * @returns A base64-encoded string representation of the key.
   */
  private static async exportKey(key: CryptoKey): Promise<string> {
    const exported = await window.crypto.subtle.exportKey('raw', key);
    return btoa(String.fromCharCode(...new Uint8Array(exported)));
  }

  /**
   * Imports a base64-encoded string as a CryptoKey.
   * @param keyStr The base64-encoded string representation of the key.
   * @returns The imported CryptoKey object.
   */
  private static async importKey(keyStr: string): Promise<CryptoKey> {
    const binary = atob(keyStr);
    const keyData = new Uint8Array(binary.split('').map((char) => char.charCodeAt(0)));
    return window.crypto.subtle.importKey('raw', keyData, { name: 'AES-GCM' }, true, ['encrypt', 'decrypt']);
  }

  /**
   * Stores the derived key securely in localStorage.
   * @param key The CryptoKey to store.
   */
  static async storeKey(key: CryptoKey): Promise<void> {
    const keyStr = await this.exportKey(key);
    sessionStorage.setItem(environment.storage_key, keyStr);
  }

  /**
   * Retrieves the stored key from localStorage.
   * @returns The CryptoKey object or null if no key is stored.
   */
  public static async getKey(): Promise<CryptoKey | null> {
    const keyStr = sessionStorage.getItem(environment.storage_key);
    if (!keyStr) {
      return null;
    }
    return this.importKey(keyStr);
  }
}
