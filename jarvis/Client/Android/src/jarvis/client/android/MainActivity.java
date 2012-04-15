package jarvis.client.android;

import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.io.UnsupportedEncodingException;

import org.apache.http.HttpResponse;
import org.apache.http.client.*;
import org.apache.http.client.methods.HttpPost;
import org.apache.http.entity.StringEntity;
import org.apache.http.impl.client.DefaultHttpClient;
import org.json.JSONException;
import org.json.JSONObject;
import org.w3c.dom.Document;

import android.app.Activity;
import android.content.res.Configuration;
import android.os.AsyncTask;
import android.os.Bundle;
import android.view.*;
import android.widget.TextView;



public class MainActivity extends Activity {
	
	private TextView deviceIdTextView;
	
    /** Called when the activity is first created. */
    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.main);
        
        deviceIdTextView = (TextView)findViewById(R.id.deviceId);
        
    }
    @Override
    public void onConfigurationChanged(Configuration newConfig) {
    	// TODO Auto-generated method stub
    	super.onConfigurationChanged(newConfig);
    }
    
    public void registerDevice_onClick(View view)
    {
    	AsyncTask<Void, Void, Integer> execute = new RegisterAsync()
    	{
    		@Override
    		protected void onPostExecute(Integer result) {
    			// TODO Auto-generated method stub
    			deviceIdTextView.setText(String.valueOf(result));
    		}
    	};
    	execute.execute();
    	
    }
    
    public class RegisterAsync extends AsyncTask<Void, Void, Integer>
    {


		@Override
		protected Integer doInBackground(Void... params) {
			
			HttpClient httpClient = new DefaultHttpClient();
	    	
	    	HttpPost post = new HttpPost("http://10.140.0.35:5368/Services/ClientService.svc/client");
	    	post.setHeader("Accept", "application/json");
	    	post.setHeader("Content-type", "application/json");
	    	
	    	JSONObject request = new JSONObject();
	    	try {
				request.put("Hostname", "Nexus S");
				request.put("Name", "Nexus S");
				request.put("TypeValue", 4);
	    	} catch (JSONException e1) {
				// TODO Auto-generated catch block
				e1.printStackTrace();
			}
	    	
	    	try {
				post.setEntity(new StringEntity(request.toString()));
			} catch (UnsupportedEncodingException e1) {
				// TODO Auto-generated catch block
				e1.printStackTrace();
			}
	    	
	    	try {
				HttpResponse response = httpClient.execute(post);
				InputStream content = response.getEntity().getContent();
				StringBuilder sb = new StringBuilder();
				
				try{
					BufferedReader reader = new BufferedReader(new InputStreamReader(content,"iso-8859-1"),8);
					
					String line = null;
					while ((line = reader.readLine()) != null) {
						sb.append(line + "\n");
					}
					content.close();

				}catch(Exception e){
				}
				
				try {
					JSONObject result = new JSONObject(sb.toString());
					
					return result.getInt("Id");
					
				} catch (JSONException e) {
					// TODO Auto-generated catch block
					e.printStackTrace();
				}
				
				
			} catch (ClientProtocolException e) {
				// TODO Auto-generated catch block
				e.printStackTrace();
			} catch (IOException e) {
				// TODO Auto-generated catch block
				e.printStackTrace();
			}
			
	    	return -1;
		}
    	
    }
    
}