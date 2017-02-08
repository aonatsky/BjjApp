package ua.org.bjj.dto;

import javax.persistence.*;
import java.sql.Time;

@Entity
public class Fight {
    @Id
    @Column(name = "id", unique = true)
    @SequenceGenerator(name="FIGHT_ID_GENERATOR", sequenceName="\"fight_id_seq\"", allocationSize = 1)
    @GeneratedValue(strategy= GenerationType.SEQUENCE, generator="FIGHT_ID_GENERATOR")
    private Long id;

    @ManyToOne
    @JoinColumn(name = "blue_gi_fighter_id", foreignKey = @ForeignKey(name = "FIGHTER_PK_ID"))
    private Fighter blueGiFighterId;

    @ManyToOne
    @JoinColumn(name = "white_gi_fighter_id", foreignKey = @ForeignKey(name = "FIGHTER_PK_ID"))
    private Fighter whiteGiFighterId;

    @Column
    private Time timeCompleted;

    @Column
    private Boolean isCompleted;

    @Column
    private String result;

    @ManyToOne
    @JoinColumn(name = "winner_id", foreignKey = @ForeignKey(name = "FIGHTER_PK_ID"))
    private Fighter winnerId;

    @ManyToOne
    @JoinColumn(name = "fight_list_id", foreignKey = @ForeignKey(name = "FIGHT_LIST_PK_ID"))
    FightList fightListId;

}
