import axios from 'axios';
import React, { Component } from 'react';

export class Home extends Component {
    constructor(props) {
        super(props);
        this.state = {
            // Put form values here
            // Bootstrap elements: https://getbootstrap.com/docs/4.3/components/forms/
            selectedFile: null
        };

        this.handleChange = this.handleChange.bind(this);
        this.handleSubmit = this.handleSubmit.bind(this);
    }

    onFileChange = event => {
        // Update the state
        this.setState({ selectedFile: event.target.files[0] });
    };

    onFileUpload = () => {
        // Create an object of formData
        const formData = new FormData();

        // Update the formData object
        formData.append(
            "files",
            this.state.selectedFile,
            this.state.selectedFile.name
        );

        for (var [key, value] of formData.entries()) {
            console.log(key, value);
        }

        console.log("Selected file: " + this.state.selectedFile);

        axios.post("https://localhost:44385/upload/UploadFile", formData, {
            headers: {
                "Content-Type": "multipart/form-data",
                "Accept": "text/plain",
            },
        })
    };

    handleChange(event) {
        const target = event.target;
        const value = target.type === 'checkbox' ? target.checked : target.value;
        const name = target.name;

        this.setState({
            [name]: value
        });
    }

    handleSubmit(event) {

        const target = event.target;
        const value = target.type === 'checkbox' ? target.checked : target.value;
        const name = target.name;

        this.setState({
            [name]: value
        });

        alert('A name was submitted: ' + this.state.selectedFile
        );
        event.preventDefault();
    }

    render() {
        return (
            <div>
                <h2>React Form</h2>
                <form onSubmit={this.handleSubmit}>
                    <div className="form-group">
                        <label htmlFor="formFileMultiple" className="form-label">Multiple files input example</label>
                        <input className="form-control" type="file" id="formFileMultiple" multiple onChange={this.onFileChange} />
                    </div>
                    <div className="form-group">
                        <br />
                        <button type="submit" className="btn btn-primary" value="Submit" onClick={this.onFileUpload}>Submit!</button>
                    </div>
                </form>
            </div>
        );
    }
}