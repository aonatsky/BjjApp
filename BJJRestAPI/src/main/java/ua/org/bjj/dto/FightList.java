package ua.org.bjj.dto;

import javax.persistence.*;

@Entity
@Table(name = "fight_list")
public class FightList {
    @Id
    @Column(name = "id", unique = true)
    @SequenceGenerator(name="FIGHT_LIST_ID_GENERATOR", sequenceName="\"fight_list_id_seq\"", allocationSize = 1)
    @GeneratedValue(strategy= GenerationType.SEQUENCE, generator="FIGHT_LIST_ID_GENERATOR")
    private Long id;

    @ManyToOne
    @JoinColumn(name = "age_class_id", foreignKey = @ForeignKey(name = "AGE_CLASS_PK_ID"))
    private AgeClass ageClassId;

    @ManyToOne
    @JoinColumn(name = "belt_class_id", foreignKey = @ForeignKey(name = "BELT_CLASS_PK_ID"))
    private BeltClass beltClassId;

    //TODO: talk it through with DB architect
//    private Division divisionId;

    @ManyToOne
    @JoinColumn(name = "tournament_id", foreignKey = @ForeignKey(name = "TOURNAMENT_PK_ID"))
    private Tournament tournamentId;

    @ManyToOne
    @JoinColumn(name = "weight_class_id", foreignKey = @ForeignKey(name = "WEIGHT_CLASS_PK_ID"))
    private WeightClass weightClass;
}
