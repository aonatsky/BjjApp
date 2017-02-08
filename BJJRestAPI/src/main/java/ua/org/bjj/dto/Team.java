package ua.org.bjj.dto;

import javax.persistence.*;

@Entity
@Table(name = "team")
public class Team {
    @Id
    @Column(name = "id", unique = true)
    @SequenceGenerator(name="TEAM_ID_GENERATOR", sequenceName="\"team_id_seq\"", allocationSize = 1)
    @GeneratedValue(strategy=GenerationType.SEQUENCE, generator="TEAM_ID_GENERATOR")
    private Long id;

    @Column
    private String description;

    @Column
    private String name;
}
