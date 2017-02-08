package ua.org.bjj.dto;

import javax.persistence.*;

@Entity
@Table(name = "weight_class")
public class WeightClass {
    @Id
    @Column(name = "id", unique = true)
    @SequenceGenerator(name="WEIGHT_CLASS_ID_GENERATOR", sequenceName="\"weight_class_id_seq\"", allocationSize = 1)
    @GeneratedValue(strategy=GenerationType.SEQUENCE, generator="WEIGHT_CLASS_ID_GENERATOR")
    private Long id;

    @Column
    private String description;

    @Column
    private String name;

    @Column
    private Integer weight;
}
