package ua.org.bjj.dto;

import javax.persistence.*;

@Entity
@Table (name = "age_class")
public class AgeClass {
    @Id
    @Column(name = "id", unique = true)
    @SequenceGenerator(name="AGE_CLASS_ID_GENERATOR", sequenceName="\"age_class_id_seq\"", allocationSize = 1)
    @GeneratedValue(strategy= GenerationType.SEQUENCE, generator="AGE_CLASS_ID_GENERATOR")
    private Long id;

    @Column
    private Integer age;

    @Column
    private String name;
}
