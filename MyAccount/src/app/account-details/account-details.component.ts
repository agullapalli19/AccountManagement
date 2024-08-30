import { Component, OnInit } from '@angular/core';
import { MasterService } from '../master.service';
import { AbstractControl, FormControl, FormGroup, ValidatorFn, Validators } from '@angular/forms';
import { EncryptionService } from '../_services/EncryptionService';

export interface AccountDetails {
  Id: number;
  AccountHolderName: string;
  Balance: number;
}

export function nonNegativeValidator(): ValidatorFn {
  return (control: AbstractControl): { [key: string]: any } | null => {
    const value = control.value;
    return (isNaN(value) || value < 0) ? { 'negative': true } : null;
  };
}

export function destinationMatchValidator(): ValidatorFn {
  return (formGroup: AbstractControl): { [key: string]: any } | null => {
    const destination = formGroup.get('destinationAccountNumber')?.value;
    const confirmDestination = formGroup.get('destinationAccountNumber2')?.value;

    return destination && confirmDestination && destination !== confirmDestination
      ? { 'destinationMismatch': true }
      : null;
  };
}


@Component({
  selector: 'app-account-details',
  templateUrl: './account-details.component.html',
  styleUrls: ['./account-details.component.css']
})
export class AccountDetailsComponent implements OnInit {
  account: any;
  errorMessage = "";
  successMessage = "";
  publicKey: string = '';
  UserDetails: any;
  

  myForm = new FormGroup({
    sourceAccountNumber: new FormControl({
      value: '', disabled: true
    }, [Validators.required, Validators.minLength(8), Validators.maxLength(16)]),
    destinationAccountNumber: new FormControl('', [Validators.required, Validators.minLength(8), Validators.maxLength(16)]),
    destinationAccountNumber2: new FormControl('', [Validators.required, Validators.minLength(8), Validators.maxLength(16)]),
    amountTotransfer: new FormControl('', [Validators.required, Validators.min(1), nonNegativeValidator()]),
    transactionDescription: new FormControl('')
  }, [destinationMatchValidator(), this.transferAmountValidator()]);

  constructor(private service: MasterService, private encryptionService: EncryptionService) { }

  transferAmountValidator(): ValidatorFn {
    return (formGroup: AbstractControl): { [key: string]: any } | null => {
      const transferAmount = formGroup.get('amountTotransfer')?.value;
      const balanceAmount = this.account?.balance;

      return transferAmount && transferAmount > balanceAmount
        ? { 'insufficientBalance': true }
        : null;
    };
  }

  ngOnInit(): void {
    this.UserDetails = this.encryptionService.getUserDetails();
    this.service.getAccountDetails(this.UserDetails.userId).subscribe(result => {
      this.account = result;
    });
    
  }


  onSubmit() {
    //debugger
    
    const publicKey = this.encryptionService.getPublicKey();

    if (this.myForm.valid) {
      var payload: any = {
        PersonId: this.UserDetails.userId,
        SourceAccountNumber: this.account?.accountNumber ?? '',
        TargetAccountNumber: this.myForm.value.destinationAccountNumber ?? '',
        Amount: this.myForm.value.amountTotransfer ?? 0,
        transactionDescription: this.myForm.value.transactionDescription ?? '',
      }

      const encryptedData: string = this.encryptionService.encryptData(payload, publicKey);
      payload = {
        encryptedData: encryptedData
      };
      // let payload = {sourceAccountNumber:s}
      this.service.accountTransfer(payload).subscribe(result => {
        //     debugger
        if (result == 222) {
          this.successMessage = "";
          this.errorMessage = "The Account you are trying to transfer to is invalid. Please verify the account number. Transaction once made cannot be reverted back."
        }
        else {
          this.account = result;
          this.myForm.reset();
          this.successMessage = "Transaction has been completed. Your account should reflect the new balance";
        }
      }, (error => {
        console.log(error, error);
        this.successMessage = "";
        this.errorMessage = "We're having trouble completing the transaction. Please try after sometime. If you continue to have issues with the transfer, please contact support center"

      }));
    } else {
      console.log('Form is invalid.');
    }
  }


}
