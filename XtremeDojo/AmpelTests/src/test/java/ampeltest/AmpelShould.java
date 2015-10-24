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

    private HttpRequest createGetRequest(String path) throws IOException {
        GenericUrl url = new GenericUrl("http://192.168.1.105:3000" + path);
        return requestFactory.buildGetRequest(url);
    }

    private HttpRequest createPostRequest(String path) throws IOException {
        GenericUrl url = new GenericUrl("http://192.168.1.105:3000" + path);
        return requestFactory.buildPostRequest(url, null);
    }


    private AmpelState executeGet(String command) throws Exception {
        HttpRequest request = createGetRequest(command);
        AmpelState result =  request.execute().parseAs(AmpelState.class);
        Thread.sleep(400);
        return result;
    }

    private AmpelState executePost(String command) throws Exception {
        HttpRequest request = createPostRequest(command);
        AmpelState result =  request.execute().parseAs(AmpelState.class);
        Thread.sleep(400);
        return result;
    }


    @Before
    public void setUp() throws Exception {
        Thread.sleep(100);
        executePost("/ampel/reset");
    }

    @Test
    public void redIsOffAfterReset() throws Exception {
        AmpelState state = executeGet("/ampel");
        Assert.assertFalse(state.red);
    }

    @Test
    public void redIsOn() throws Exception {
        AmpelState state =  executePost("/ampel/red/on");
        Assert.assertTrue(state.red);
        Assert.assertFalse(state.green);
        Assert.assertFalse(state.yellow);
    }

    @Test
    public void redIsOff() throws Exception {
        executePost("/ampel/red/on");
        AmpelState state =  executePost("/ampel/red/off");
        Assert.assertFalse(state.red);
    }

    @Test
    public void yellowIsOn() throws Exception {
        AmpelState state =  executePost("/ampel/yellow/on");
        Assert.assertTrue(state.yellow);
    }

    @Test
    public void yellowIsOff() throws Exception {
        executePost("/ampel/yellow/on");
        AmpelState state =  executePost("/ampel/yellow/off");
        Assert.assertFalse(state.yellow);
    }

    @Test
    public void greenIsOn() throws Exception {
        AmpelState state =  executePost("/ampel/green/on");
        Assert.assertTrue(state.green);
    }

    @Test
    public void greenIsOff() throws Exception {
        executePost("/ampel/green/on");
        AmpelState state =  executePost("/ampel/green/off");
        Assert.assertFalse(state.green);
    }

    @Test(expected = HttpResponseException.class)
    public void badColorOff() throws Exception {
        AmpelState state =  executePost("/ampel/badcolor/off");
    }

    @Test(expected = HttpResponseException.class)
    public void badState() throws Exception {
        AmpelState state =  executePost("/ampel/red/badState");
    }


    @After
    public void tearDown() throws Exception {
        executePost("/ampel/reset");
    }
}
