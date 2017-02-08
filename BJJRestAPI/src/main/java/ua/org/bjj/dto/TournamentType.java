package ua.org.bjj.dto;

import javax.persistence.*;

@Entity
@Table(name = "TOURNAMENT_TYPE")
public class TournamentType {
    @Id
    @Column(name = "id", unique = true)
    @SequenceGenerator(name="TOURNAMENT_TYPE_ID_GENERATOR", sequenceName="\"tournament_type_id_seq\"", allocationSize = 1)
    @GeneratedValue(strategy=GenerationType.SEQUENCE, generator="TOURNAMENT_TYPE_ID_GENERATOR")
    private Long id;

    @Column
    private String description;

    @Column
    private String name;
}
