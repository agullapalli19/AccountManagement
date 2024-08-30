import { Injectable } from '@angular/core';
import * as forge from 'node-forge';
import {jwtDecode} from 'jwt-decode';

@Injectable({
    providedIn: 'root',
})
export class EncryptionService {
    UserDetails: any;
    decodedToken: any;
    private publicKey: string = '';
    constructor() { }

    encryptData(data: any, publicKey: string): string {
        const pemKey = `-----BEGIN PUBLIC KEY-----\n${publicKey}\n-----END PUBLIC KEY-----`;
        const rsa = forge.pki.publicKeyFromPem(pemKey);
        const json = JSON.stringify(data);
        const encrypted = rsa.encrypt(json, 'RSA-OAEP', {
            md: forge.md.sha256.create()
        });
        return forge.util.encode64(encrypted);
    }

    getUserDetails(){
        this.decodedToken = jwtDecode(sessionStorage.getItem("jwtToken")??'');
        return {username: this.decodedToken.unique_name, userId: Number(this.decodedToken.upn), givenName:  this.decodedToken.given_name};        
    }

    getPublicKey() {
        return this.publicKey;
    }
 
    setPublicKey(key: string) {
        this.publicKey = key;
    }
}