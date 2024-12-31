import {jwtDecode, JwtPayload} from 'jwt-decode';
import {CustomJwtPayload} from '../../domain/custom-jwt-payload';
import {TokenData} from '../../domain/domain';

export class TokenParser {

  static parseToken(token: string): TokenData | null {
    try {
      const decodedToken = jwtDecode<CustomJwtPayload>(token);

      const id = decodedToken["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"]; // Custom claim for user ID
      const expires = decodedToken.exp; // Standard exp claim (expiration time)
      const notBefore = decodedToken.nbf; // Standard nbf claim (not before)
      const issuer = decodedToken.iss;
      const audience = decodedToken.aud;

      if (id && expires && notBefore && issuer && audience) {
        return {
          id: parseInt(id),
          exp: new Date(expires * 1000),
          nbf: new Date(notBefore * 1000),
          iss: issuer,
          aud: audience
        };
      }
      return null;
    } catch (error) {
      console.error('Error parsing JWT token');
      return null;
    }
  }
}
