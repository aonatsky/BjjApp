package ua.org.bjj.dto;

import javax.persistence.*;

@Entity
@Table(name = "fighter")
public class Fighter {
    @Id
    @Column(name = "id", unique = true)
    @SequenceGenerator(name="FIGHTER_ID_GENERATOR", sequenceName="\"fighter_id_seq\"", allocationSize = 1)
    @GeneratedValue(strategy=GenerationType.SEQUENCE, generator="FIGHTER_ID_GENERATOR")
    private Long id;

    @Column(name = "first_name")
    private String firstName;

    @Column(name = "last_name")
    private String lastName;

    @ManyToOne
    @JoinColumn(name = "team_id", foreignKey = @ForeignKey(name = "TEAM_PK_ID"))
    private Team teamId;

}
