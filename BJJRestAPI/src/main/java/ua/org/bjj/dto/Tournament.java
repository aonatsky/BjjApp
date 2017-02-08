package ua.org.bjj.dto;

import javax.persistence.*;
import java.sql.Date;

@Entity
public class Tournament {
    @Id
    @Column(name = "id", unique = true)
    @SequenceGenerator(name="TOURNAMENT_ID_GENERATOR", sequenceName="\"tournament_id_seq\"", allocationSize = 1)
    @GeneratedValue(strategy=GenerationType.SEQUENCE, generator="TOURNAMENT_ID_GENERATOR")
    private Long id;

    @Column
    private Date date;

    @Column
    private String description;

    @ManyToOne
    @JoinColumn(name = "owner_id", foreignKey = @ForeignKey(name = "OWNER_PK_ID"))
    private Owner ownerId;
}
