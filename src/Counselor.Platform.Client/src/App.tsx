import React from 'react';
import logo from './assets/logo.svg';
import './styles/App.css';
import { BrowserRouter as Router, Route } from 'react-router-dom';

import { PrivateRoute } from './common/PrivateRoute';
import { HomeView } from './views/home/HomeView';
import { LoginView } from './views/login/LoginView';

function App() {
  return (
    <div className="App">
      <header className="App-header">
        <Router>
          <div>
            <PrivateRoute exact path="/" component={HomeView} />
            <Route path="/login" component={LoginView}/>
          </div>
        </Router>        
      </header>
    </div>
  );
}

export default App;
