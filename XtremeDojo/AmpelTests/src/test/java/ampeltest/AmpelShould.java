package ampeltest;

import com.google.api.client.http.*;
import com.google.api.client.http.javanet.NetHttpTransport;
import com.google.api.client.json.JsonFactory;
import com.google.api.client.json.JsonObjectParser;
import com.google.api.client.json.jackson2.JacksonFactory;
import com.google.api.client.util.Key;
import org.junit.After;
import org.junit.Assert;
import org.junit.Before;
import org.junit.Test;

import java.io.IOException;

/**
 * Created by Dimitry on 24.10.2015.
 */
public class AmpelShould {
    static final HttpTransport HTTP_TRANSPORT = new NetHttpTransport();
    static final JsonFactory JSON_FACTORY = new JacksonFactory();
    HttpRequestFactory requestFactory =
            HTTP_TRANSPORT.createRequestFactory(new HttpRequestInitializer() {
                @Override
                public void initialize(HttpRequest request) {
                    request.setParser(new JsonObjectParser(JSON_FACTORY));
                }
            });

    public static class AmpelState{
        @Key public boolean red;
        @Key public boolean green;
        @Key public boolean yellow;

    }

    private HttpRequest getHttpRequest(String path) throws IOException {
        GenericUrl url = new GenericUrl("http://192.168.1.105:3000" + path);
        return requestFactory.buildGetRequest(url);
    }


    private AmpelState execute(String command) throws Exception {
        HttpRequest request = getHttpRequest(command);
        AmpelState result =  request.execute().parseAs(AmpelState.class);
        Thread.sleep(400);
        return result;
    }


    @Before
    public void setUp() throws Exception {
        Thread.sleep(100);
        execute("/ampel/reset");
    }

    @Test
    public void redIsOffAfterReset() throws Exception {
        AmpelState state = execute("/ampel");
        Assert.assertFalse(state.red);
    }

    @Test
    public void redIsOn() throws Exception {
        AmpelState state =  execute("/ampel/red/on");
        Assert.assertTrue(state.red);
        Assert.assertFalse(state.green);
        Assert.assertFalse(state.yellow);
    }

    @Test
    public void redIsOff() throws Exception {
        execute("/ampel/red/on");
        AmpelState state =  execute("/ampel/red/off");
        Assert.assertFalse(state.red);
    }

    @Test
    public void yellowIsOn() throws Exception {
        AmpelState state =  execute("/ampel/yellow/on");
        Assert.assertTrue(state.yellow);
    }

    @Test
    public void yellowIsOff() throws Exception {
        execute("/ampel/yellow/on");
        AmpelState state =  execute("/ampel/yellow/off");
        Assert.assertFalse(state.yellow);
    }

    @Test
    public void greenIsOn() throws Exception {
        AmpelState state =  execute("/ampel/green/on");
        Assert.assertTrue(state.green);
    }

    @Test
    public void greenIsOff() throws Exception {
        execute("/ampel/green/on");
        AmpelState state =  execute("/ampel/green/off");
        Assert.assertFalse(state.green);
    }

    @Test(expected = HttpResponseException.class)
    public void badColorOff() throws Exception {
        AmpelState state =  execute("/ampel/badcolor/off");
    }

    @Test(expected = HttpResponseException.class)
    public void badState() throws Exception {
        AmpelState state =  execute("/ampel/red/badState");
    }


    @After
    public void tearDown() throws Exception {
        execute("/ampel/reset");
    }
}
