package ua.org.bjj.initializer;

import org.eclipse.jetty.server.Server;
import org.eclipse.jetty.servlet.ServletContextHandler;
import org.eclipse.jetty.servlet.ServletHolder;
import org.springframework.web.context.ContextLoaderListener;
import org.springframework.web.context.WebApplicationContext;
import org.springframework.web.context.support.AnnotationConfigWebApplicationContext;
import org.springframework.web.servlet.DispatcherServlet;

import java.io.IOException;

public class JettyInitializer {
    private static final int DEFAULT_PORT = 8080;
    private static final String CONTEXT_PATH = "/";
    private static final String CONFIG_LOCATION = "ua.org.bjj.config";
    private static final String MAPPING_URL = "/*";
    private static final String DEFAULT_PROFILE = "dev";

    private static Integer port;
    private static String profile;


    public static void main(String[] args) throws Exception {
        parseAgs(args);

        new JettyInitializer().startJetty(port);
    }

    /**
     * Helper-method to process arguments of the application.
     * Argument #1: an integer type int representing port.
     * Argument #2: a string representing profile name.
     * @param args
     */
    private static void parseAgs(String... args) {
        try {
            port = Integer.valueOf(args[0]);
            profile = args[1];
        } catch (NumberFormatException ignore) {
        } catch (ArrayIndexOutOfBoundsException ignore) {
        } finally {
            if (port == null) port = DEFAULT_PORT;
            if (profile == null) profile = DEFAULT_PROFILE;
        }


    }

    private void startJetty(int port) throws Exception {
        Server server = new Server(port);
        server.setHandler(getServletContextHandler(getContext()));
        server.start();
        server.join();
    }

    private static WebApplicationContext getContext() {
        AnnotationConfigWebApplicationContext contextHandler = new AnnotationConfigWebApplicationContext();
        contextHandler.setConfigLocation(CONFIG_LOCATION);
        contextHandler.getEnvironment().setDefaultProfiles(DEFAULT_PROFILE);

        if (profile != DEFAULT_PROFILE) {
            contextHandler.getEnvironment().setActiveProfiles(profile);
        }

        return contextHandler;
    }

    private static ServletContextHandler getServletContextHandler(WebApplicationContext context) throws IOException {
        ServletContextHandler contextHandler = new ServletContextHandler();
        contextHandler.setErrorHandler(null);
        contextHandler.setContextPath(CONTEXT_PATH);
        contextHandler.addServlet(new ServletHolder(new DispatcherServlet(context)), MAPPING_URL);
        contextHandler.addEventListener(new ContextLoaderListener(context));
        return contextHandler;
    }

}
