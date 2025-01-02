import {environment} from '../../environments/environment';
import {fromPromise} from 'rxjs/internal/observable/innerFrom';
import {Observable} from 'rxjs';

export class Hasher {

    static hash(password: string, salt: string): Observable<string> {
      const rounds = environment.salt_rounds;
      return fromPromise(this.hashForRoundsAsync(password, salt, rounds));
    }

  static hashPromise(password: string, salt: string): Promise<string> {
      const rounds = environment.salt_rounds;
      return this.hashForRoundsAsync(password, salt, rounds);
  }

  private static async hashForRoundsAsync(password: string, salt: string, rounds: number): Promise<string> {
    let hashed = await this.hashAsync(password, salt);
    for (let i = 0; i < rounds; i++) {
      hashed = await this.hashAsync(hashed, salt);
    }
    return hashed;
  }

  private static async hashAsync(password: string, salt: string): Promise<string> {
    const enc = new TextEncoder();
    const keyMaterial = await window.crypto.subtle.importKey(
      "raw",
      enc.encode(password),
      "PBKDF2",
      false,
      ["deriveBits", "deriveKey"]
    );
    const key = await window.crypto.subtle.deriveKey(
      {
        name: "PBKDF2",
        salt: enc.encode(salt),
        iterations: 100000,
        hash: "SHA-256"
      },
      keyMaterial,
      { name: "HMAC", hash: "SHA-256", length: 256 },
      true,
      ["sign"]
    );
    const exportedKey = await window.crypto.subtle.exportKey("raw", key);
    return btoa(String.fromCharCode(...new Uint8Array(exportedKey)));
  }
}
