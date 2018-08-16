import { Component, OnInit } from '@angular/core';
import { FederationModel } from '../../core/model/federation.model';
import { FederationService } from '../../core/services/federation.service';

@Component({
  selector: 'federation-edit',
  templateUrl: './federation-edit.component.html',
  styleUrls: ['./federation-edit.component.scss']
})
export class FederationEditComponent implements OnInit {
  model: FederationModel;
  constructor(private federationService: FederationService) {}

  ngOnInit() {
    this.federationService.getFederation().subscribe(r =>{
      this.model = r;}
      );
  }

  save() {
    this.federationService.updateFederation(this.model).subscribe();
  }
}
