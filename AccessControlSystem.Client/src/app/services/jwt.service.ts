import { Injectable } from '@angular/core';

export interface JwtPayload {
  userId: string;
  userName: string;
  email: string;
  subscriptionId: string;
  role: string;
  exp: number;
  iss: string;
  aud: string;
}

@Injectable({
  providedIn: 'root'
})
export class JwtService {

  constructor() { }

  /**
   * Decodes a JWT token and returns the payload
   * @param token The JWT token to decode
   * @returns The decoded payload or null if invalid
   */
  decodeToken(token: string): JwtPayload | null {
    try {
      const base64Url = token.split('.')[1];
      const base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
      const jsonPayload = decodeURIComponent(atob(base64).split('').map(function(c) {
        return '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2);
      }).join(''));

      return JSON.parse(jsonPayload);
    } catch (error) {
      console.error('Error decoding JWT token:', error);
      return null;
    }
  }

  /**
   * Gets the current user information from the stored token
   * @returns User information or null if no valid token
   */
  getCurrentUser(): { userName: string; email: string; role: string; userId: string } | null {
    const token = localStorage.getItem('authToken');
    
    if (!token) {
      return null;
    }

    const payload = this.decodeToken(token);
    
    if (!payload) {
      return null;
    }

    return {
      userName: payload.userName,
      email: payload.email,
      role: payload.role,
      userId: payload.userId
    };
  }

  /**
   * Checks if the current token is expired
   * @returns true if token is expired or invalid
   */
  isTokenExpired(): boolean {
    const token = localStorage.getItem('authToken');
    if (!token) {
      return true;
    }

    const payload = this.decodeToken(token);
    if (!payload) {
      return true;
    }

    const currentTime = Math.floor(Date.now() / 1000);
    return payload.exp < currentTime;
  }
}
