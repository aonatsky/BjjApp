import { Component, Input, OnInit, OnDestroy } from '@angular/core';
import { timer } from 'rxjs';
import { MatchModel } from '../../core/model/match.models';
import { MatchDetailsModel } from '../../core/model/match-details.model';
import { EventRunCommunicationService } from '../../core/hubservices/event-run.communication.service';

@Component({
  selector: 'round-panel-view',
  templateUrl: './round-panel-view.component.html',
  styleUrls: ['./round-panel-view.component.scss']
})
export class RoundPanelViewComponent implements OnInit, OnDestroy {
  @Input() matchModel: MatchModel;
  @Input() title: string;
  matchDetails: MatchDetailsModel;

  private readonly tick: number;
  private timerSubscription: any;

  constructor(private eventRunCommunicationService: EventRunCommunicationService) {
    this.matchDetails = new MatchDetailsModel();
    this.matchDetails.aParticipantPenalties = 0;
    this.matchDetails.bParticipantPenalties = 0;
    this.matchDetails.aParticipantAdvantages = 0;
    this.matchDetails.bParticipantAdvantages = 0;
    this.matchDetails.aParticipantPoints = 0;
    this.matchDetails.bParticipantPoints = 0;

    this.matchDetails.countdown = 2 * 60;
    this.matchDetails.isStarted = false;
    this.matchDetails.isPaused = false;
    this.matchDetails.isCompleted = false;

    this.tick = 1000;
  }

  ngOnInit(): void {
    this.matchDetails.countdown = this.matchModel.matchTime;
    this.matchDetails.matchId = this.matchModel.matchId;
    this.eventRunCommunicationService.onMatchDetailsUpdate().subscribe(data => {
      this.matchDetails = data;

      if (this.matchDetails.isStarted) {
        this.startTimer();
      }
      if (this.matchDetails.isPaused || this.matchDetails.isCompleted) {
        this.stopTimer();
      }
    });
  }

  ngOnDestroy(): void {
   console.log("DESTROYED")
  }

  private startTimer(): void {
    if (!this.timerSubscription) {
      this.timerSubscription = timer(0, this.tick).subscribe(() => {
        --this.matchDetails.countdown;
        if (this.matchDetails.countdown <= 0) {
          this.stopTimer();
        }
      });
    }
  }

  private stopTimer(): void {
    if (this.timerSubscription) {
      this.timerSubscription.unsubscribe();
    }
    this.timerSubscription = null;
  }
}
