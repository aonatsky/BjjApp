package ua.org.bjj.dto;

import javax.persistence.*;

@Entity
@Table(name = "belt_class")
public class BeltClass {
    @Id
    @Column(name = "id", unique = true)
    @SequenceGenerator(name="BELT_CLASS_ID_GENERATOR", sequenceName="\"belt_class_id_seq\"", allocationSize = 1)
    @GeneratedValue(strategy= GenerationType.SEQUENCE, generator="BELT_CLASS_ID_GENERATOR")
    private Long id;

    @Column
    private String name;
}
