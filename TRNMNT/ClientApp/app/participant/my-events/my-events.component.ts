import { Component, OnInit } from '@angular/core';
import { ParticipantEventModel } from '../../core/model/participant.models';
import { ParticipantService } from '../../core/services/participant.service';
import { SelectItem } from '../../../../node_modules/primeng/primeng';
import { WeightDivisionSimpleModel } from '../../core/model/weight-division.models';
import { WeightDivisionService } from '../../core/services/weight-division.service';
import { CategoryService } from '../../core/services/category.service';

@Component({
  selector: 'my-events',
  templateUrl: './my-events.component.html',
  styleUrls: ['./my-events.component.scss']
})
export class MyEventsComponent implements OnInit {
  participants: ParticipantEventModel[];
  participationData: ParticipationData[] = [];
  weightDivisionsSelectItems: SelectItem[];
  weightDivisions: WeightDivisionSimpleModel[];

  constructor(
    private participantService: ParticipantService,
    private categoryService: CategoryService,
    private weightDivisionService: WeightDivisionService
  ) {}

  ngOnInit() {
    this.participantService.getUserParticipations().subscribe(r => {
      this.participants = r;
      this.participants.forEach(p => {
        const pData = new ParticipationData();
        this.categoryService.getCategoriesByEventId(p.eventId).subscribe(r => {
          pData.categorySelectItems = [];
          r.forEach(c => pData.categorySelectItems.push({ label: c.name, value: c.categoryId }));
        });
        this.weightDivisionService.getWeightDivisionsByCategory(p.categoryId).subscribe(w => {
          pData.weightDivisionsSelectItems = [];
          w.forEach(wd => {
            pData.weightDivisionsSelectItems.push({ label: wd.name, value: wd.weightDivisionId });
          });
        });
        pData.participant = p;
        this.participationData.push(pData);
      });
    });
  }

  update(model: ParticipantEventModel) {
    this.participantService.updateParticipant(model).subscribe();
  }

  initWeightDivisionDropdown(participationData: ParticipationData) {
    this.weightDivisionService.getWeightDivisionsByCategory(participationData.participant.categoryId).subscribe(w => {
      participationData.weightDivisionsSelectItems = [];
      w.forEach(wd => {
        participationData.weightDivisionsSelectItems.push({ label: wd.name, value: wd.weightDivisionId });
      });
    });
  }
}
export class ParticipationData {
  participant: ParticipantEventModel;
  weightDivisionsSelectItems: SelectItem[];
  categorySelectItems: SelectItem[];
  isValid(): boolean {
    return (
      this.participant.firstName &&
      this.participant.lastName &&
      this.participant.categoryId &&
      this.participant.weightDivisionId &&
      !!this.participant.dateOfBirth
    );
  }
}
