import {Fighter} from "./fighter.model"

export /**
 * Fight
 */
    class Fight {
    fightID: AAGUID;
    level : number;
    nextFightID: AAGUID;
    whiteGiFighterID: AAGUID;
    blueGiFighterID: AAGUID;  
    fightListID: AAGUID;
    isCompleted: boolean;
    winnerID:AAGUID;
    result:string;

    whiteGiFighter: Fighter;
    blueGiFighter: Fighter;

    constructor(whiteGifighter:Fighter, blueGifighter:Fighter) {
        this.whiteGiFighter = whiteGifighter;
        this.blueGiFighter = blueGifighter;
        this.level = 0;
        this.isCompleted = false;
    }
}