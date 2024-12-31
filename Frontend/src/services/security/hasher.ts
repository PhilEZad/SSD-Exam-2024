import {environment} from '../../environments/environment';
import * as bcrypt from 'bcryptjs';
import {fromPromise} from 'rxjs/internal/observable/innerFrom';
import {Observable} from 'rxjs';

export class Hasher {

    static hash(password: string): Observable<string> {
      const rounds = environment.salt_rounds;
      return fromPromise(bcrypt.hash(password, rounds));
    }
}
