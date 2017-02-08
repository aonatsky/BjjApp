package ua.org.bjj.utils;


import org.apache.log4j.Logger;

import java.io.FileInputStream;
import java.io.IOException;
import java.util.Properties;

public class PropertiesUtils {

    private static Logger logger = Logger.getLogger(PropertiesUtils.class);

    public static Properties getProperties(String filepath) {
        FileInputStream fis;
        Properties property = new Properties();

        try {
            fis = new FileInputStream(filepath);
            property.load(fis);
        } catch (IOException e) {
            e.printStackTrace();
        } finally {
            return property;
        }
    }
}
