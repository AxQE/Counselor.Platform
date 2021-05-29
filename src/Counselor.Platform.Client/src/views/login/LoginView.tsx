import React, { FormEventHandler } from 'react'
import Logger from '../../services/Logger'
import { userService } from '../../services/UserService';


interface IProps{}
interface IState{
    username?: string;
    password?: string;
    submitted: boolean;
    loading: boolean;
    error?: string;
}

class LoginView extends React.Component<IProps, IState> {

    private _logger = Logger.Logger.getInstance();

    constructor(props: IProps) {
        super(props);
        this.state = {
            username: '',
            password: '',
            submitted: false,
            loading: false,
            error: ''
        }

        this.HandleChangeUsername = this.HandleChangeUsername.bind(this);
        this.HandleChangePassword = this.HandleChangePassword.bind(this);
        this.HandleSubmit = this.HandleSubmit.bind(this);
    }

    HandleChangeUsername(e:any){
        const {value} = e.target;
        this.setState({username: value});        
    }

    HandleChangePassword(e:any){
        const {value} = e.target;
        this.setState({password: value});
    }

    HandleSubmit(e:any){
        e.preventDefault();

        this.setState({submitted: true});
        const {username, password} = this.state;

        if (!(username && password)) return;

        userService.login(username, password);        
    }

    render() {
        const { username, password, submitted, loading, error } = this.state;
        return (
            <div className="login-view">
                <form onSubmit={this.HandleSubmit}>
                    <fieldset>
                        <fieldset>
                            <input
                            className="form-control"
                            type="username"
                            placeholder="Username"
                            value={username}
                            onChange={this.HandleChangeUsername} />
                        </fieldset>
                        <fieldset>
                            <input
                            className="form-control"
                            type="password"
                            placeholder="Password"
                            value={password}
                            onChange={this.HandleChangePassword} />
                        </fieldset>
                        <button
                            className="btn btn-lg btn-primary pull-xs-right"
                            type="submit"
                            disabled={this.state.loading}>
                            Sign in
                        </button>
                    </fieldset>
                </form>
            </div>
        );
    }
}

export { LoginView }